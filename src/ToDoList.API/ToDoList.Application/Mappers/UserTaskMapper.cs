using AutoMapper;
using ToDoList.Application.Dto.UserTask;
using ToDoList.Core.Entities;

namespace ToDoList.Application.Mappers
{
    public class UserTaskMapper : Profile
    {
        public UserTaskMapper()
        {
            CreateMap<CreateUserTaskDto, UserTask>().ReverseMap();
            CreateMap<UpdateUserTaskDto,UserTask>().ReverseMap();
            CreateMap<UserTaskResponseDto, UserTask>().ReverseMap();
        }
    }
}
