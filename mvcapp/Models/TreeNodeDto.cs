using System.Collections.Generic;

namespace Models
{

    /// <summary>
    /// Класс описывающие узлы дерева для отображения в представлениях
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNodeDto<T> where T : new()
    {
        public TreeNodeDto() { }
        public TreeNodeDto(long id, string name = null, string nodeType = null, long? parentId = null)
        {
            Id = id;
            Name = name;
            NodeType = nodeType;
            ParentId = parentId;
        }

        /// <summary>
        /// Id элемента
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Отображение в дереве
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип узла (пр-р Contract, BillObject)
        /// </summary>
        public string NodeType { get; set; }

        /// <summary>
        /// Id родителя, если есть
        /// </summary>
        public long? ParentId { get; set; }

        public TreeNodeDto<T> Parent { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; } = new T();

        /// <summary>
        /// Дочерние элементы этого же дерева
        /// </summary>
        public virtual IList<TreeNodeDto<T>> NodeItems { get; set; } = new List<TreeNodeDto<T>>();
    }
}
