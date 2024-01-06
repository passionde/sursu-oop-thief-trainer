using EngineLibrary;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ThiefHackUI
{

    public class Offset
    {
        public Offset(uint typeParameter, uint memoryOffset)
        {
            TypeParameter = typeParameter;
            MemoryOffset = memoryOffset;
        }

        [Key]
        public uint TypeParameter { get; set; }
        public uint MemoryOffset { get; set; }
    }

    class DatabaseService : IDatabaseService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        public DatabaseService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            // Создаёт базу данных, если она отсутствует
            using var context = _contextFactory.CreateDbContext();
            context.Database.EnsureCreated();

            // Инициализация начальных значений
            if (!context.Offset.Any())
            {
                context.Offset.Add(new Offset(typeParameter: EngineHelper.AMMO_PARAMETR, memoryOffset: EngineHelper.AMMO_OFFSET));
                context.Offset.Add(new Offset(typeParameter: EngineHelper.MONEY_PARAMETR, memoryOffset: EngineHelper.MONEY_OFFSET));
                context.SaveChanges();
            }

        }

        public void EditOffsetByType(uint type, uint offset)
        {
            using var context = _contextFactory.CreateDbContext();
            Offset? offsetObj = context.Offset.FirstOrDefault(offset => offset.TypeParameter == type);

            if (offsetObj == null) { return; }

            offsetObj.MemoryOffset = offset;
            context.SaveChanges();
        }

        public uint GetOffsetByType(uint type)
        {
            using var context = _contextFactory.CreateDbContext();
            Offset? offsetObj = context.Offset.FirstOrDefault(offset => offset.TypeParameter == type);

            if (offsetObj == null) { return 0; }
            return offsetObj.MemoryOffset;
        }
    }
}
