using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Learning
{
    public class Course : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? SourceTemplateId { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? ZoneCode { get; set; }

        /// <summary>Ruta relativa (subida por el admin) del audio ambiental que suena al entrar a la región.</summary>
        public string? AmbientAudioUrl { get; set; }
        /// <summary>Nombre de la pista, mostrado en el reproductor (ej. "Huayno de los Andes").</summary>
        public string? AmbientAudioTitle { get; set; }
        /// <summary>Link externo a la canción/playlist en Spotify (botón "Escuchar en Spotify").</summary>
        public string? SpotifyUrl { get; set; }
    }
}
