using System.Text.Json.Serialization;

namespace MoreWag;

public class Config
{
    [JsonInclude] public double SpeedStep = 0.005;
}
