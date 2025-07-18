using Folklore.Logical;

namespace Folklore.Syntax;

public class Assignment : SyntaxNode
{
    public override List<SyntaxRule>? Rules { get; }
    public Reference AssignTo { get; set; }
    public string? AssignedConstantLiteral { get; private set; }
    public Reference? AssignedReference { get; private set; }

    public Assignment(Reference assignTo)
    {
        AssignTo = assignTo;
    }

    protected override void OnTokenAdded(Token token)
    {
        if (Tokens.Any(x => x.Kind == TokenKind.EndOfLine)) return;
        if (token.Kind == TokenKind.Literal && AssignedConstantLiteral == null)
        {
            AssignedConstantLiteral = token.Text;
        }
        else if (token.Kind == TokenKind.Identifier && AssignedReference == null)
        {
            AssignedReference = new Reference(token.Text);
        }
    }

    public override bool IsValid(out string[] errors)
    {
        bool hasVariable = AssignTo != null;

        if (!hasVariable)
        {
            errors = new[] { "Assignment must have a variable to assign to." };
            return false;
        }

        bool hasValue = AssignedConstantLiteral != null || AssignedReference != null;
        if (!hasValue)
        {
            errors = new[] { "Assignment must have a value to assign." };
            return false;
        }

        errors = [];
        return true;
    }
}