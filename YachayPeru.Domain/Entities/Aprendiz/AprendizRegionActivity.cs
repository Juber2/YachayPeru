using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    /// <summary>
    /// Última región/módulo que un aprendiz vio (una fila por usuario, se sobrescribe
    /// cada vez que llama a GET aprendiz/regiones/{id}). Alimenta "ultimaActividad" en
    /// el resumen de perfil / home.
    /// </summary>
    public class AprendizRegionActivity : BaseEntity
    {
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public int ModuleId { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
