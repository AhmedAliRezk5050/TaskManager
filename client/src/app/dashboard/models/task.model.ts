import {TaskStatus} from "../enums/task-status.enum";

export interface ITask {
  id: number
  title: string
  description: string
  status: TaskStatus
  dueDate: string
  createdAt: string
  updatedAt?: string
  createdById: string
  updatedById?: string
  createdByName: string
  updatedByName?: string
}
