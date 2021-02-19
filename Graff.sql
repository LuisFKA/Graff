CREATE TABLE [dbo].[Pessoa] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Nome]  NVARCHAR (MAX) NULL,
    [Idade] INT            NOT NULL,
    CONSTRAINT [PK_Pessoa] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Produto] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Nome]  NVARCHAR (MAX) NULL,
    [Valor] REAL           NOT NULL,
    CONSTRAINT [PK_Produto] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Lance] (
    [Id]        INT  IDENTITY (1, 1) NOT NULL,
    [Valor]     REAL NOT NULL,
    [PessoaId]  INT  NOT NULL,
    [ProdutoId] INT  NOT NULL,
    CONSTRAINT [PK_Lance] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Lance_Pessoa_PessoaId] FOREIGN KEY ([PessoaId]) REFERENCES [dbo].[Pessoa] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Lance_Produto_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [dbo].[Produto] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_Lance_PessoaId]
    ON [dbo].[Lance]([PessoaId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Lance_ProdutoId]
    ON [dbo].[Lance]([ProdutoId] ASC);

-- Query para obtenção das informações dos lances salvos com as informações do produto que teve o lance, e a pessoa que fez o lance.
SELECT l.Id, pr.Nome, l.Valor, pe.Nome FROM (dbo.Lance l INNER JOIN dbo.Pessoa pe ON l.PessoaId = pe.Id) INNER JOIN dbo.Produto pr ON l.ProdutoId = pr.Id;