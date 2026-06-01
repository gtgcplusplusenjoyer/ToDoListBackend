using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoList.Core.Entities;
using ToDoList.Core.Enums;
using ToDoList.Core.Models;

namespace ToDoList.Infrastructure.Extensions
{
    public static class UserTaskExtension
    {
        public static IQueryable<UserTask> Filter(this IQueryable<UserTask> query, UserTaskFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(t => t.Name.Contains(filter.Name));
            }

            if (filter.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == filter.Priority.Value);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            if (filter.DueTime.HasValue)
            {
                query = query.Where(t => t.DueDate != null && t.DueDate.Value.Date.Day == filter.DueTime.Value.Day);
            }

            return query;
        }

        public static IQueryable<UserTask> Sort(this IQueryable<UserTask> query, SortParams sortParams)
        {
            if (sortParams.SortDirection == SortDirection.Desc)
            {
                return query.OrderByDescending(GetKeySelector(sortParams.OrderBy));
            }

            return query.OrderBy(GetKeySelector(sortParams.OrderBy));
        }

        public static async Task<PagedResult<UserTask>> ToPagedAsync(this IQueryable<UserTask> query, PageParams pageParams)
        {
            var count = await query.CountAsync();

            if (count == 0)
            {
                return new PagedResult<UserTask>([], 0);
            }

            var page = pageParams.Page ?? 1;
            var pageSize = pageParams.PageSize ?? 10;

            var skip = (page - 1) * pageSize;

            var result = await query
                .Skip(skip)
                .Take(pageSize)
                .ToArrayAsync();

            return new PagedResult<UserTask>(result, count);
        }

        private static Expression<Func<UserTask, object>> GetKeySelector(string? OrderBy)
        {
            if (string.IsNullOrEmpty(OrderBy))
            {
                return t => t.Name;
            }

            return OrderBy switch
            {
                nameof(UserTask.Priority) => t => t.Priority,
                nameof(UserTask.Status) => t => t.Status,
                nameof(UserTask.DueDate) => t => t.DueDate,
                _ => t => t.Name
            };

        }
    }
}
