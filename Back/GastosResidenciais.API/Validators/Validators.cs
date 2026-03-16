using FluentValidation;
using GastosResidenciais.Application.DTOs;

namespace GastosResidenciais.API.Validators;

// ── Pessoa Validators ─────────────────────────────────────────────────────────

/// <summary>
/// Validador para criação de pessoa.
/// FluentValidation permite expressar regras de forma fluente e legível,
/// com mensagens de erro em português para o cliente.
/// </summary>
public class CreatePessoaValidator : AbstractValidator<CreatePessoaDto>
{
    public CreatePessoaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Idade)
            .GreaterThanOrEqualTo(0).WithMessage("A idade não pode ser negativa.")
            .LessThanOrEqualTo(150).WithMessage("Idade inválida.");
    }
}

/// <summary>Validador para atualização de pessoa — mesmas regras de criação.</summary>
public class UpdatePessoaValidator : AbstractValidator<UpdatePessoaDto>
{
    public UpdatePessoaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Idade)
            .GreaterThanOrEqualTo(0).WithMessage("A idade não pode ser negativa.")
            .LessThanOrEqualTo(150).WithMessage("Idade inválida.");
    }
}

// ── Categoria Validators ──────────────────────────────────────────────────────

/// <summary>Validador para criação de categoria.</summary>
public class CreateCategoriaValidator : AbstractValidator<CreateCategoriaDto>
{
    public CreateCategoriaValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(400).WithMessage("A descrição deve ter no máximo 400 caracteres.");

        RuleFor(x => x.Finalidade)
            .IsInEnum().WithMessage("Finalidade inválida. Use: 0=Despesa, 1=Receita, 2=Ambas.");
    }
}

// ── Transacao Validators ──────────────────────────────────────────────────────

/// <summary>
/// Validador para criação de transação.
/// Valida a estrutura do DTO — as regras de negócio (menor de idade,
/// compatibilidade de categoria) são validadas na camada de Application.
/// </summary>
public class CreateTransacaoValidator : AbstractValidator<CreateTransacaoDto>
{
    public CreateTransacaoValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(400).WithMessage("A descrição deve ter no máximo 400 caracteres.");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("O valor deve ser positivo (maior que zero).");

        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("Tipo inválido. Use: 0=Despesa, 1=Receita.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("A categoria é obrigatória.");

        RuleFor(x => x.PessoaId)
            .NotEmpty().WithMessage("A pessoa é obrigatória.");
    }
}
