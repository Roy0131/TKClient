public class ResPathHelper
{
    public static string CONFIG = "configs/";
    public static string ROLE_PATH = "roles/";
    public static string UI_PATH = "uiprefabs/";
    public static string EFFECT_PATH = "effect/";

    public static string GetXmlPath(string fileName)
    {
        return CONFIG + fileName;
    }

    public static string GetRolePath(string roleName)
    {
        return ROLE_PATH + roleName;
    }

    public static string GetUIPath(string uiPrefab)
    {
        return UI_PATH + uiPrefab;
    }

    public static string GetItemIconPath(string iconName)
    {
        return "itemicon/" + iconName;
    }

    public static string GetEffectPath(string effName)
    {
        return EFFECT_PATH + effName;
    }

    public static string GetUIEffectPath(string effName)
    {
        return "uieffect/" + effName;
    }

    public static string GetRoleIconPath(string name)
    {
        return "roleicon/" + name;
    }

    public static string GetMapPath(string name)
    {
        return "maps/" + name;
    }

    public static string GetCampPath(string name)
    {
        return "campicon/" + name;
    }

    public static string GetSkillIconPath(string name)
    {
        return "skillicon/" + name;
    }

    public static string GetGuildIconPath(string name)
    {
        return "guildicon/" + name;
    }

    public static string GetBuffIconPath(string name)
    {
        return "bufficon/" + name;
    }

    public static string GetArtifactTexturePath(string name)
    {
        return "artifacticon/" + name;
    }

    public static string GetSoundClipPath(string name)
    {
        return "sound/" + name;
    }
}