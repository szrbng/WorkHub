using FluentValidation;

namespace WorkHub.Application.Features.Auth.Login
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
        //Login’de şifre kurallarını (büyük harf vb.) kontrol etmiyoruz, sadece boş olmasın diyoruz. Kuralları değiştirdiğinde eski kullanıcılar giriş yapamaz hale gelmesin.
    }
}
