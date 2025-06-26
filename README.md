# Clube da Leitura 📚 
 
Este é um sistema de gerenciamento de empréstimos de revistas e caixas, desenvolvido em C#, com foco em cadastro, visualização, exclusão, edição e controle de devoluções

## Sumário

- [Visão geral](#visão-geral)
  - [Mídia](#mídia-)
  - [Funcionalidades](#funcionalidades)
  - [Desenvolvido com](#desenvolvido-com-)
  - [Estrutura do projeto](#estrutura-do-projeto-)
- [Como rodar o código?](#como-rodar-o-código-)
  - [Passo a passo - Clone ou baixe o projeto](#passo-a-passo---clone-ou-baixe-o-projeto--)
  - [Uso](#uso-)
- [Autor](#autor-)

## Visão geral

### Mídia 📷
##### GIF da aplicação - Clique no GIF para dar Play/Pause
![image](https://imgur.com/r1SNu5g.gif)

##### Módulo de Multas e Reservas
![image](https://i.imgur.com/GhV6fjR.gif)

### Funcionalidades principais ✅ 
- Cadastro de Amigos

   - Nome

   - Nome do responsável

   - Telefone

   - Endereço

- Cadastro de Caixas

   - Etiqueta

   - Cor (seleção por paleta)

   - Número

- Cadastro de Revistas

   - Título

   - Número da edição

   - Ano

   - Caixa onde está armazenada

- Gerenciamento de Empréstimos

   - Registro de empréstimos (revista + amigo)

   - Cálculo de data de devolução automática

   - Registro de devoluções

   - Validação de disponibilidade da revista
  
   - Bloqueio de múltiplos empréstimos por amigo
   
- Multas por atraso
   - Geração automática de multa no momento da devolução atrasada
   - Visualização de multas pendentes
   - Quitação de multas por ID

- **Sistema de Reservas**
   - Criação de reservas para revistas
   - Impede o empréstimo de revistas reservadas
   - Cancelamento de reservas ativas
   - Indicação de status "Reservada" na visualização de revistas

- **Recursos adicionais**
   - Edição de cadastros com possibilidade de manter valores
   - Visualização em formato de tabela
   - Exclusão de registros por ID
   - Mensagens interativas no terminal
   - Paleta de cores amigável para caixas

- Recursos adicionais

   - Edição de cadastros com possibilidade de manter valores

   - Visualização em formato de tabela

   - Exclusão de registros por ID

   - Mensagens interativas no terminal

   - Paleta de cores amigável para caixas


### Desenvolvido com 🚀

[![My Skills](https://skillicons.dev/icons?i=cs,dotnet,git&theme=light)](https://skillicons.dev)


### Estrutura do projeto 📁
```
├── ClubedaLeitura.ConsoleApp
│   ├── Apresentação
│   │   └── TelaMenuPrincipal.cs
│   ├── Compartilhado
│   │   ├── EntidadeBase.cs
│   │   ├── EntradaHelper.cs
│   │   ├── IEntidade.cs
│   │   ├── RepositorioBase.cs
│   │   ├── TelaBase.cs
│   │   └── TelaMenuEntidadeBase.cs
│   ├── Configuracoes
│   │   └── Configuracao.cs
│   ├── ModuloAmigo
│   │   ├── Amigo.cs
│   │   ├── RepositorioAmigo.cs
│   │   └── TelaAmigo.cs
│   ├── ModuloCaixa
│   │   ├── Caixa.cs
│   │   ├── PaletaCores.cs
│   │   ├── RepositorioCaixa.cs
│   │   ├── SeletorDeCor.cs
│   │   └── TelaCaixa.cs
│   ├── ModuloEmprestimo
│   │   ├── Emprestimo.cs
│   │   ├── ImprimirEmprestimo.cs
│   │   ├── Multa.cs
│   │   ├── RepositorioEmprestimo.cs
│   │   └── TelaEmprestimo.cs
│   ├── ModuloReserva
│   │   ├── RepositorioReserva.cs
│   │   ├── Reserva.cs
│   │   └── TelaReserva.cs
│   ├── ModuloRevista
│   │   ├── RepositorioRevista.cs
│   │   ├── Revista.cs
│   │   └── TelaRevista.cs
│   ├── Utils
│   │   ├── DesejaExcluir.cs
│   │   ├── DigitarEnterEContinuar.cs
│   │   ├── Direcionar.cs
│   │   ├── ResultadoDirecionamento.cs
│   │   └── Validar
│   │       └── ValidaCampo.cs
│   ├── Program.cs
│   └── ClubedaLeitura.ConsoleApp.csproj
├── .gitattributes
├── .gitignore
├── ClubedaLeitura.sln
└── README.md

```


### Como rodar o código? 🤖

#### ❗❗Obs: Há a necessidade de ter o .NET SDK instalado em sua máquina previamente!

#### Passo a passo - Clone ou baixe o projeto  👣👣

1. Abra o terminal do seu editor de código;
2. Navegue até a pasta onde deseja instalar o projeto;
3. Clone o projeto 
ex:``` git clone git@github.com:alexandreSouza31/clube_da_leitura.git```
 ou se preferir, baixe clicando no botão verde chamado "Code" no repositório desse projeto, e depois "Download zip.


#### Uso 💻
1. Inicie o App:
Certifique-se de estar na pasta do projeto, e navegue pelo terminal até o caminho do arquivo Program.cs
```
cd clube-da-leitura/ClubedaLeitura.ConsoleApp
```
2. Compile e execute o programa: ```dotnet run```

    ou, com o arquivo Program.cs aberto clique no botão verde(Current Document(Program.cs)) para iniciar

3. Siga as instruções do menu interativo no terminal!


## Autor 😏 

<main>
<div style="display: flex; align-items: center; gap: 20px;padding-bottom: 2em">
  <img src="https://github.com/user-attachments/assets/74c712a4-9e48-4ae3-839c-46089b850a27" width="80" />
  <h3 style="margin: 0;"><i>Alexandre Mariano</i></h4>
</div>

  <p>
    <a href="https://www.linkedin.com/in/alexandresouza31/">
      <img src="https://skillicons.dev/icons?i=linkedin&theme=dark" width="50"/>
      LinkedIn
    </a> &nbsp;  |  &nbsp;
    <a href="https://github.com/alexandreSouza31">
      <img src="https://skillicons.dev/icons?i=github&theme=dark" width="50"/>
      GitHub
    </a>
  </p>
</main>


<a href="#clube-da-leitura" 
   style="position: fixed; right: 10px; bottom: 20px; background-color:rgba(99, 102, 99, 0.32); color: white; padding: 1px 5px 5px; text-decoration: none; border-radius: 5px; font-size: 16px;">
   🔝Voltar ao topo🔝
</a>
