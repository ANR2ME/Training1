﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class ItemRepository : EfRepository<Item>, IItemRepository
    {

        private StockControlEntities entities;
        public ItemRepository()
        {
            entities = new StockControlEntities();
        }

        public IList<Item> GetAll()
        {
            return FindAll().ToList();
        }

        public Item GetObjectById(int Id)
        {
            Item item = Find(x => x.Id == Id && !x.IsDeleted);
            if (item != null) { item.Errors = new Dictionary<string, string>(); }
            return item;
        }

        public Item CreateObject(Item item)
        {
            item.IsDeleted = false;
            item.CreatedAt = DateTime.Now;
            return Create(item);
        }

        public Item UpdateObject(Item item)
        {
            item.UpdatedAt = DateTime.Now;
            Update(item);
            return item;
        }

        public Item SoftDeleteObject(Item item)
        {
            item.IsDeleted = true;
            item.DeletedAt = DateTime.Now;
            Update(item);
            return item;
        }

        public bool DeleteObject(int Id)
        {
            Item item = Find(x => x.Id == Id);
            return (Delete(item) == 1) ? true : false;
        }
    }
}
