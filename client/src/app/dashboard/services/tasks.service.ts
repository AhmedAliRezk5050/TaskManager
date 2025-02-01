import {Injectable} from "@angular/core";
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";
import {IPagedList} from "../../shared/models/paged-list";
import {ITask} from "../models/task.model";
import {ICreateTaskRequest} from "../models/create-task-request";
import {IUpdateTaskRequest} from "../models/update-task-request";
import {MessageService} from "primeng/api";

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private apiUrl = `${environment.apiUrl}/tasks`;
   private pagedTasksSubject  =
    new BehaviorSubject<IPagedList<ITask>>({items: [], totalCount: 0});

   pagedTasks$ = this.pagedTasksSubject.asObservable();

  constructor(private http: HttpClient, private messageService: MessageService) {}

  private fetchTasks(page: number = 1, pageSize: number = 10): Observable<IPagedList<ITask>> {
    return this.http.get<IPagedList<ITask>>(`${this.apiUrl}?PageNumber=${page}&PageSize=${pageSize}`);
  }

  createTask(createTaskRequest: ICreateTaskRequest): Observable<void> {
    return this.http.post<void>(this.apiUrl, createTaskRequest);
  }

  updateTask(id: number, updateTaskRequest: IUpdateTaskRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, updateTaskRequest);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getTasks(page: number = 1, pageSize: number = 10) {
    this.fetchTasks(page, pageSize).subscribe({
      next: fetchedPagedTasks => {
        this.pagedTasksSubject.next(fetchedPagedTasks);
      },
      error: () => {
        this.pagedTasksSubject.next({items: [], totalCount: 0});
        this.messageService.add({ severity: 'error', summary: 'Loading tasks Failed', detail: 'Try again' });
      }
    })
  }
}
