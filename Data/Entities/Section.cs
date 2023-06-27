namespace ASP_111.Data.Entities
{
    public class Section
    {
        public Guid      Id          { get; set; }
        public String    Title       { get; set; } = null!;
        public String    Description { get; set; } = null!;
        public String?   ImageUrl    { get; set; }
        public Guid      AuthorId    { get; set; }
        public DateTime  CreateDt    { get; set; }
        public DateTime? DeleteDt    { get; set; }

        // Navigation props
        public User Author { get; set; }
    }
}
/* Навигационные свойства - свойства ({ get; set; }) в сущности, которые
 * ссылаются на другие сущности (или их коллекции)
 * Используются для отображения связей таблиц БД и упрощения доступа к
 * связанным данным.
 * 
 * Автоматически настраиваются если имя поля соотв. таблице
 * UserId - авто связь с табл. User, полем Id,
 * иначе требуется настройка в контексте (см. класс DataContext)
 * 
 * Связываются (извлекаются) инструкцией .Include (см. ForumController)
 */
