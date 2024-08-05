namespace WebAppHero.Persistence.DependencyInjection.Options;

public class PasswordValidatorOptions
{
    public int RequiredMinLength { get; set; }

    public int RequiredNonAlphanumericLength { get; set; }

    public int RequiredLowercaseLength { get; set; }

    public int RequiredUppercaseLength { get; set; }

    public int RequiredDigitLength { get; set; }
}
