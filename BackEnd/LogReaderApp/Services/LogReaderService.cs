using LogReaderApp.Data;
using LogReaderApp.Models;
using LogReaderApp.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogReaderApp.Services
{
    public class LogReaderService : ILogReaderService
    {
        private readonly LogReaderDbContext _context;

        public LogReaderService(LogReaderDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Log> GetAll()
        {
            return _context.Log.Include(x => x.User).ToList();
        }
        public async Task<Log> Get(int id)
        {
            var log = await _context.Log.FindAsync(id);
            log.User = _context.User.FirstOrDefault(x => x.IP == log.IP);
            return log;
        }
        public async Task<IEnumerable<Log>> GetFiltered(FilterInput input)
        {
             var log = await _context.Log
                .Where(x => string.IsNullOrEmpty(input.Ip) ? true : x.IP == input.Ip)
                .Where(x => string.IsNullOrEmpty(input.User) ? true : x.UserId.ToString() == input.User)
                .ToListAsync();
            return log;
        }
        public async Task CreateBatch(Lote input)
        {
            foreach (var item in input.Logs)
            {
                var user = _context.User.FirstOrDefault(x => x.IP == item.IP);
                if (user != null)
                    item.UserId = user.Id;
                else
                {
                    user = new User();
                    user.IP = item.IP;
                    user.Nome = "User IP " + item.IP;
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();
                    item.UserId = user.Id;
                }
                _context.Log.Add(item);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<int> Create(Log input)
        {
            var user = _context.User.FirstOrDefault(x => x.IP == input.IP);
            if (user != null)
                input.UserId = user.Id;
            else
            {
                user = new User();
                user.IP = input.IP;
                user.Nome = "User IP " + input.IP;
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                input.UserId = user.Id;
            }
            _context.Log.Add(input);
            await _context.SaveChangesAsync();
            return input.Id;
        }
        public async Task<bool> Update(int id, Log input)
        {
            _context.Entry(input).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }
        }
        public async Task Delete(int id)
        {
            var log = await _context.Log.FindAsync(id);
            _context.Log.Remove(log);
            await _context.SaveChangesAsync();
        }
       
    }
}
