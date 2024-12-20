using CrudDapper.Dto;
using CrudDapper.Models;

namespace CrudDapper.Services
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<List<UsuarioListaDto>>> BuscarUsuarios();
        Task<ResponseModel<UsuarioListaDto>> BuscarUsuarioPorId(int usuarioId);

        Task<ResponseModel<List<UsuarioListaDto>>> CriarUsuario(UsuarioCriarDto usuarioCriarDto);
        Task<ResponseModel<List<UsuarioListaDto>>> EditarUsuario(UsuarioEditarDto usuarioEditarDto);

        Task<ResponseModel<List<UsuarioListaDto>>> RemoverUsuario(int usuarioId);

    }
}
