using Unity;

namespace Chat.Core.DependencyInjection
{
    public interface IDependencyInstaller
    {
        void Install(IUnityContainer container);
    }
}
