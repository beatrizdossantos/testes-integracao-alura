using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.CoisasAFazer.Testes
{
    public class RepositorioFake : IRepositorioTarefas
    {
        List<Tarefa> listaTarefas = new List<Tarefa>();

        public void AtualizarTarefas(params Tarefa[] tarefas)
        {
            throw new NotImplementedException();
        }

        public void ExcluirTarefas(params Tarefa[] tarefas)
        {
            throw new NotImplementedException();
        }

        public void IncluirTarefas(params Tarefa[] tarefas)
        {
            tarefas.ToList().ForEach(tarefa => listaTarefas.Add(tarefa));
        }

        public Categoria ObtemCategoriaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tarefa> ObtemTarefas(Func<Tarefa, bool> filtro)
        {
            return listaTarefas.Where(filtro);
        }
    }
}
