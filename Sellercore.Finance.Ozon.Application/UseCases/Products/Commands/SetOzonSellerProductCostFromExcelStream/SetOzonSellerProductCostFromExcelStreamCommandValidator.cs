using FluentValidation;

namespace Sellercore.Finance.Ozon.Application.UseCases.Products.Commands.SetOzonSellerProductCostFromExcelStream;

public class SetOzonSellerProductCostFromExcelStreamCommandValidator
    : AbstractValidator<SetOzonSellerProductCostFromExcelStreamCommand>
{
    public SetOzonSellerProductCostFromExcelStreamCommandValidator()
    {
        // Проверка на то, что файл не пустой
        RuleFor(x => x.ExcelFile)
            .NotNull()
            .WithMessage("File is required.")
            .Must(file => file.Length > 0)
            .WithMessage("File is empty.");

        // Проверка расширения файла
        RuleFor(x => x.ExcelFile)
            .Must(file => IsExcelFile(file.FileName))
            .WithMessage("Invalid file extension. Only .xlsx or .xls files are allowed.");
    }

    private bool IsExcelFile(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return extension == ".xlsx" || extension == ".xls";
    }
}