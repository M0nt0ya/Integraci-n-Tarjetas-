namespace Kiosko.Domain.Entities
{
    public interface ITransactionKiosko
    {
        public int Id { get;  }
        public int IdLocal { get;  }
        public string Direccion { get;  }
        public string Email { get;  }
        public string Clave { get;  }
        public string Puerto { get;  }
        public string TokenAcceso { get;  }
        public DateTime TokenFechaCaduca { get;  }
        public DateTime TokenFechaCreado { get;  }
        public int Estado { get;  }
    }
}