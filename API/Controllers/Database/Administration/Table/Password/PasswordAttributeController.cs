﻿using API.Entity.Database.Administration.Table.Password;

namespace API.Controllers.Database.Administration.Table.Password
{
    public class PasswordAttributeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "PasswordAttribute";

        public PasswordAttributeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<PasswordAttributeEntity>();
        }

        internal PasswordAttributeEntity GetActiveEntityByDescription(string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'");
            return new PasswordAttributeEntity(dataRow);
        }
    }
}
