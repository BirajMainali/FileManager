﻿namespace FileManager.Domain.Entities.Interfaces
{
    public interface IRecordInfo
    {
        public User.User RecUser { get; set; }
        public long RecUserId { get; set; }
    }
}