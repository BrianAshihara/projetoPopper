﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace popper.app.Models
{
    internal class ChamadoModel
    {
        public int Id { get; set; }

        public int idTecnico { get; set; }
        public int idUsuario { get; set; }      
        
        public int idLocal { get; set; }
        public string descricao { get; set; }

        public string status { get; set; }

       
    }
}
