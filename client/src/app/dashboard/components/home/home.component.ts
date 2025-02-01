import {Component, OnInit} from '@angular/core';
import {DragDropModule} from "primeng/dragdrop";
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {TasksService} from "../../services/tasks.service";
import {IPagedList} from "../../../shared/models/paged-list";
import {ITask} from "../../models/task.model";
import {TaskStatus} from "../../enums/task-status.enum";
import {ButtonDirective} from "primeng/button";
import {ConfirmDialogModule} from "primeng/confirmdialog";
import {ConfirmationService} from "primeng/api";
import {DialogModule} from "primeng/dialog";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {InputTextModule} from "primeng/inputtext";
import {CalendarModule} from "primeng/calendar";
import {InputTextareaModule} from "primeng/inputtextarea";
import {ICreateTaskRequest} from "../../models/create-task-request";
import {IUpdateTaskRequest} from "../../models/update-task-request";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    DragDropModule,
    NgForOf,
    DatePipe,
    ButtonDirective,
    ConfirmDialogModule,
    DialogModule,
    ReactiveFormsModule,
    InputTextModule,
    NgIf,
    CalendarModule,
    NgClass,
    InputTextareaModule,
    FormsModule
  ],
  providers: [ConfirmationService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  pagedTasks: IPagedList<ITask> = {items: [], totalCount: 0};
  draggedTask: ITask | null = null;
  newTaskDialogVisible = false;
  editTaskDialogVisible = false;

  newTask: ICreateTaskRequest = { title: '', description: '', status: 1, dueDate: '' };
  editableTask: IUpdateTaskRequest = { title: '', description: '', status: 1, dueDate: '' };
  editableTaskId = 1;

  constructor(
    private tasksService: TasksService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder
  ) { }


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

  confirmDelete(task: ITask) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${task.title}"?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.deleteTask(task.id!);
      }
    });
  }

  deleteTask(taskId: number) {
    this.tasksService.deleteTask(taskId).subscribe({
      next: () => {
        this.tasksService.getTasks(); // Refresh task list
      },
      error: () => {
      }
    });
  }

  openNewTaskDialog(status: number) {
    this.newTask = { title: '', description: '', status, dueDate: new Date().toISOString().split('T')[0] };
    this.newTaskDialogVisible = true;
  }

  createTask() {
    if (!this.newTask.title.trim()) {
      return;
    }

    this.tasksService.createTask(this.newTask).subscribe({
      next: () => {
        this.tasksService.getTasks(); // Refresh the task list
        this.newTaskDialogVisible = false;
      },
      error: () => {
      }
    });
  }

  openEditTaskDialog(task: ITask) {
    this.editableTask = task;
    this.editableTaskId = task.id;
    this.editableTask.dueDate = new Date(task.dueDate).toISOString().split('T')[0];
    this.editTaskDialogVisible = true;
  }

  editTask() {
    if (!this.editableTask.title.trim()) {
      return;
    }

    this.tasksService.updateTask(this.editableTaskId, this.editableTask).subscribe({
      next: () => {
        this.tasksService.getTasks();
        this.editTaskDialogVisible = false;
      },
      error: () => {
      }
    });
  }

  cancelEditTask() {
    this.editTaskDialogVisible = false
    this.tasksService.getTasks();
  }
}
