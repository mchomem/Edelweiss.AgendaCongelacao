using System;
using System.Collections.Generic;

namespace Edelweiss.AgendaCongelacao.Model.Dashboard
{
    public class AgendasAno
    {
        #region Properties

        public List<String> Meses { get; set; }

        public List<Int32> Quantidades { get; set; }

        public Int32 Total { get; set; }

        public Int32 Ano { get; set; }

        #endregion

        #region Constructors

        public AgendasAno() { }

        #endregion
    }
}
