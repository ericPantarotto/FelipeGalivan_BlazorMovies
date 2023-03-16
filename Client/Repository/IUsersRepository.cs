using BlazorMovies.Shared.DTOs;

namespace BlazorMovies.Client.Repository
{
    public interface IUsersRepository
    {
        Task AssignRole(EditRoleDTO editRole);
        Task<List<RoleDTO>?> GetRoles();
        Task<PaginatedResponse<List<UserDTO>>> GetUsers(PaginationDTO paginationDTO);
        Task RemoveRole(EditRoleDTO editRole);
    }
}
