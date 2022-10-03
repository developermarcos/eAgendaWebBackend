using System;

namespace eAgenda.Webapi.ViewModels.Depesas
{
    public class ListarDespesasViewModel
    {
        public Guid id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
