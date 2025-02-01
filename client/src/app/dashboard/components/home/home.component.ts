import {Component, OnInit} from '@angular/core';
import {DragDropModule} from "primeng/dragdrop";
import {DatePipe, NgForOf} from "@angular/common";
import {TasksService} from "../../services/tasks.service";
import {IPagedList} from "../../../shared/models/paged-list";
import {ITask} from "../../models/task.model";
import {TaskStatus} from "../../enums/task-status.enum";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    DragDropModule,
    NgForOf,
    DatePipe
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  pagedTasks: IPagedList<ITask> = {items: [], totalCount: 0};
  draggedTask: ITask | null = null;

  constructor(public tasksService: TasksService) { }


  ngOnInit(): void {
    this.tasksService.pagedTasks$.subscribe(fetchedPagedTasks => {

      this.pagedTasks = fetchedPagedTasks;
      console.log('on init', this.pagedTasks);
    })

    this.tasksService.getTasks();
  }

  get todoPagedTasks(): IPagedList<ITask> {
    const todoTasks = [...this.pagedTasks.items.filter(t => t.status === TaskStatus.ToDo)]
    return {
      items: todoTasks,
      totalCount: this.pagedTasks.totalCount - todoTasks.length
    }
  }

  get inProgressPagedTasks(): IPagedList<ITask> {
    const inProgressTasks = [...this.pagedTasks.items.filter(t => t.status === TaskStatus.InProgress)]
    return {
      items: inProgressTasks,
      totalCount: this.pagedTasks.totalCount - inProgressTasks.length
    }
  }

  get donePagedTasks(): IPagedList<ITask> {
    const doneTasks = [...this.pagedTasks.items.filter(t => t.status === TaskStatus.Done)]
    return {
      items: doneTasks,
      totalCount: this.pagedTasks.totalCount - doneTasks.length
    }
  }

  moveTask(newStatus: number) {
    console.log('drop', newStatus)
    if(this.draggedTask && newStatus !== this.draggedTask.status) {
      this.draggedTask.status = newStatus;
      this.tasksService.updateTask(this.draggedTask.id, this.draggedTask).subscribe({
        next: () => {
          this.tasksService.getTasks();
        },
        error: () => {
          this.tasksService.getTasks();
          this.draggedTask = null;
        }
      })
    }
  }

  onTaskDragged(task: ITask) {
    this.draggedTask = task;
  }



}
