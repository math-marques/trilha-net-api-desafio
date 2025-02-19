using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using System.Linq;

namespace TrilhaApiDesafio.Controllers;
//definindo a rota da API, criação da classe mediadora e herdando da controllerbase
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;
//inserindo no contexto organizador
        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }
//método utilizado para as requisições da api = GET
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Buscar o Id no banco utilizando o Entity Framework
            var tarefa = _context.Tarefas.Find(id);

            // Validando o tipo de retorno: se a tarefa for nula, o tratamento da excessão foi realizado.
            if (tarefa == null)
                return NotFound(new { Erro = "Tarefa não encontrada" });

            return Ok(tarefa);
        }
//posso requisitar buscando todas as tarefas
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Buscar todas as tarefas no banco utilizando o EF
            var tarefas = _context.Tarefas.ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Buscar as tarefas no banco utilizando o EF, que contenha o título recebido por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            // Buscar as tarefas no banco pela data
            var tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar as tarefas no banco utilizando o status recebido por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Status == status).ToList();
            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adicionar a tarefa recebida no EF e salvar as mudanças
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound(new { Erro = "Tarefa não encontrada" });

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualizar as informações da variável tarefaBanco com a tarefa recebida
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Atualizar a variável tarefaBanco no EF e salvar as mudanças
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound(new { Erro = "Tarefa não encontrada" });

            // Remover a tarefa encontrada através do EF e salvar as mudanças
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }

