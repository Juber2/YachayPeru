using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    /// <summary>
    /// Historial de regiones que un aprendiz exploró alguna vez (una fila por usuario+región,
    /// nunca se sobreescribe). A diferencia de <see cref="AprendizRegionActivity"/> (que solo
    /// guarda la última región vista), esta tabla permite contar cuántas regiones distintas
    /// exploró un usuario — usada por las insignias de exploración.
    /// </summary>
    public class AprendizRegionExplored : BaseEntity
    {
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public DateTime FirstViewedAt { get; set; }
    }
}
