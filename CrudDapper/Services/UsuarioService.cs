using System.Data.SqlClient;
using AutoMapper;
using CrudDapper.Dto;
using CrudDapper.Models;
using Dapper;

namespace CrudDapper.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
     

        public UsuarioService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public UsuarioService()
        {
        }

        [Obsolete]
        public async Task<ResponseModel<UsuarioListaDto>> BuscarUsuarioPorId(int usuarioId)
        {
            ResponseModel<UsuarioListaDto> response = new ResponseModel<UsuarioListaDto>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnetion")))
            {
                var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("select * from Usuarios where id = @Id", new { Id = usuarioId });

                if (usuarioBanco == null)
                {
                    response.Mensagem = "Nenhum Usuário Localizado!";
                    response.Status = false;
                    return response;
                }
                //Transformação Mapper
                var usuarioMapeado = _mapper.Map<UsuarioListaDto>(usuarioBanco);
                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuário localizado com Sucesso!!";
            }
            return response;
        }

        [Obsolete]
        public async Task<ResponseModel<List<UsuarioListaDto>>> BuscarUsuarios()
        {
            ResponseModel<List<UsuarioListaDto>> response = new ResponseModel<List<UsuarioListaDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnetion")))
            {
                var usuariosBanco = await connection.QueryAsync<Usuario>("select * from Usuarios");

                if (usuariosBanco.Count() == 0)
                {
                    response.Mensagem = "Nenhum usuário foi Localizado!";
                    response.Status = false;
                    return response;
                }

                //Transformação Mapper
                var usuarioMapeado = _mapper.Map<List<UsuarioListaDto>>(usuariosBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuários Localizados com Sucesso!";
                return response;
            }
        }
        [Obsolete]
        public async Task<ResponseModel<List<UsuarioListaDto>>> CriarUsuario(UsuarioCriarDto usuarioCriarDto)
        {
            ResponseModel<List<UsuarioListaDto>> response = new ResponseModel<List<UsuarioListaDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnetion")))
            {
                var usuarioBanco = await connection.ExecuteAsync("insert into Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Senha, Situacao) " + "values (@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Senha, @Situacao)", usuarioCriarDto);

                if (usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um Erro ao Criar um Usuário";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListaDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuários listados com Sucesso!";

            }
            return response;
        }

        [Obsolete]
        private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection connection)
        {
            return await connection.QueryAsync<Usuario>("select * from Usuarios");
        }
        
        [Obsolete]
        public async Task<ResponseModel<List<UsuarioListaDto>>> EditarUsuario(UsuarioEditarDto usuarioEditarDto)
        {
            ResponseModel<List<UsuarioListaDto>> response = new ResponseModel<List<UsuarioListaDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnetion")))
            {
                var usuarioBanco = await connection.ExecuteAsync("update Usuarios set NomeCompleto = @NomeCompleto," + "Email = @Email," + "Cargo = @Cargo," + "Salario = @Salario," + "CPF = @CPF," + "Situacao = @Situacao where Id = @Id", usuarioEditarDto);

                if(usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar uma Edição";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListaDto>>(usuarios);


               response.Dados = usuariosMapeados;
                response.Mensagem = "Usuários Listados com Sucesso!";

                   
            }

            return response;
        }

        public async Task<ResponseModel<List<UsuarioListaDto>>> RemoverUsuario(int usuarioId)
        {
            ResponseModel<List<UsuarioListaDto>> response = new ResponseModel<List<UsuarioListaDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnetion")))
            {
                var usuarioBanco = await connection.ExecuteAsync("delete from Usuarios where id= @Id", new { Id = usuarioId });

                if (usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar uma Edição";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListaDto>>(usuarios);


                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuários Listados com Sucesso!";

            }

            return response;
        }
    }

}
