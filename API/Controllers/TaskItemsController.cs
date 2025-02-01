using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.DTOs;
using Infrastructure.DTOs.TaskItems;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
[Route("api/tasks")]
public class TaskItemsController : ApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskItemsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDTO<List<TaskItemDTO>>>> Get([FromQuery] PagingParams pagingParams)
    {
        var tasks = await _unitOfWork.TaskItemRepository.GetAllAsync(
            include: t => t.Include(k => k.CreatedBy).Include(k => k.UpdatedBy),
            orderBy: t => t.OrderByDescending(k => k.CreatedAt),
            pagingParams: pagingParams
            );

        var taskDTOs = _mapper.Map<List<TaskItemDTO>>(tasks);

        return Ok(new PagedResultDTO<TaskItemDTO>()
        {
            Items = taskDTOs,
            TotalCount = taskDTOs.Count()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddTaskItemDTO dto)
    {
        var taskItem = _mapper.Map<TaskItem>(dto);

        _unitOfWork.TaskItemRepository.Add(taskItem);

        var isSuccess = await _unitOfWork.SaveAsync();

        if (!isSuccess)
        {
            throw new Exception("Adding task failed");
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTaskItemDTO dto)
    {
        var taskItem = await _unitOfWork.TaskItemRepository
                                        .GetFirstOrDefaultAsync(t => t.Id == id);

        if (taskItem == null)
        {
            return NotFound();
        }

        var toUpdateTaskItem = _mapper.Map(dto, taskItem);

        _unitOfWork.TaskItemRepository.Update(toUpdateTaskItem);

        var isSuccess = await _unitOfWork.SaveAsync();

        if (!isSuccess)
        {
            throw new Exception("updating task failed");
        }

        return Ok();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var taskItem = await _unitOfWork.TaskItemRepository
                                        .GetFirstOrDefaultAsync(t => t.Id == id);

        if (taskItem == null)
        {
            return NotFound();
        }

        _unitOfWork.TaskItemRepository.Remove(taskItem);

        var isSuccess = await _unitOfWork.SaveAsync();

        if (!isSuccess)
        {
            throw new Exception("Deleting task failed");
        }

        return Ok();
    }
}
