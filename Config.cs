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
        [DisplayName("Debug Mode")]
        [Description("This is a bool.")]
        [DefaultValue(true)]
        public bool DEBUG_MODE { get; set; } = true;

        [DisplayName("Glasses Setting")]
        [Description("This is an enumerable.")]
        [DefaultValue(GlassesSetting.Modern)]
        public GlassesSetting glassesSetting { get; set; } = GlassesSetting.Modern;

        public enum GlassesSetting
        {
            NONE = 0,
            Modern = 1,
        }
        [DisplayName("Enum")]
        [Description("This is an enumerable.")]
        [DefaultValue(HairstyleSetting.Ponytail)]
        public HairstyleSetting hairstyleSetting { get; set; } = HairstyleSetting.Ponytail;

        public enum HairstyleSetting
        {
            Vanilla,
            Ponytail = 10,
            Bangs_Ponytail = 20,
        }
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
