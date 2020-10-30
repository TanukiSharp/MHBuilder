using MHBuilder.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MHBuilder.Iceborne.Models
{
    public record Ability(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("level")] int Level,
        [property: JsonPropertyName("description")] LocalizableString Description
    );

    public record Skill(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("isSetBonus")] bool IsSetBonus,
        [property: JsonPropertyName("name")] LocalizableString Name,
        [property: JsonPropertyName("description")] LocalizableString Description,
        [property: JsonPropertyName("abilities")] Ability[] Abilities
    );
}
