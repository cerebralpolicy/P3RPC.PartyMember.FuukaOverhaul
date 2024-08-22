using P3RPC.PartyMember.FuukaOverhaul.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;

namespace P3RPC.PartyMember.FuukaOverhaul.Configuration
{
    public class Config : Configurable<Config>
    {
        /*
            User Properties:
                - Please put all of your configurable properties here.
    
            By default, configuration saves as "Config.json" in mod user config folder.    
            Need more config files/classes? See Configuration.cs
    
            Available Attributes:
            - Category
            - DisplayName
            - Description
            - DefaultValue

            // Technically Supported but not Useful
            - Browsable
            - Localizable

            The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
        */
        [DisplayName("Log Level")]
        [DefaultValue(LogLevel.Information)]
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        [DisplayName("Debug Mode")]
        [Description("This is a bool.")]
        [DefaultValue(false)]
        public bool DEBUG_MODE { get; set; } = false;

        [Category("Core")]
        [DisplayName("Glasses Setting")]
        [Description("This is an enumerable.")]
        [DefaultValue(GlassesSetting.Modern)]
        public GlassesSetting glassesSetting { get; set; } = GlassesSetting.Modern;

        public enum GlassesSetting
        {
            NONE = 0,
            Modern = 10,
        }

        [Category("Core")]
        [DisplayName("Hairstyle")]
        [Description("This is an enumerable.")]
        [DefaultValue(HairstyleSetting.Ponytail)]
        public HairstyleSetting hairstyleSetting { get; set; } = HairstyleSetting.Ponytail;

        public enum HairstyleSetting
        {
            Vanilla = 0,
            Ponytail = 1,
           // Bangs_Ponytail = 20,
        }

        [Category("Costumes")]
        [DisplayName("Battle Panties => Prodigal Scientist Parka")]
        [Description("Fuuk you, I'm unsexualizing your Fuuka.")]
        [DefaultValue(true)]
        public bool UNSC_Parka { get; set; } = true;

    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
