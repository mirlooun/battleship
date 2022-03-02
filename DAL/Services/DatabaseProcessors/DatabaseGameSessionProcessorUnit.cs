using System;

namespace DAL.Services.DatabaseProcessors
{
    public static class DatabaseGameSessionProcessorUnit
    {
        public static Creator GetCreator(AppDbContext context)
        {
            return new Creator(context);
        }

        public static Updater GetUpdater(AppDbContext context)
        {
            return new Updater(context);
        }
        
        public static Loader GetLoader(AppDbContext context)
        {
            return new Loader(context);
        }
    }
}
