﻿namespace CadastroProdutosFornecedores.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Recurso não encontrado.") { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}

