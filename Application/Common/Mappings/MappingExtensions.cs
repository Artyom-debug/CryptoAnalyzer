using Application.Common.Models;

namespace Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<ItemNavigator<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, CancellationToken cancellationToken = default) where TDestination : class
    => ItemNavigator<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, cancellationToken);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration, CancellationToken cancellationToken = default) where TDestination : class
    => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync(cancellationToken);
}
