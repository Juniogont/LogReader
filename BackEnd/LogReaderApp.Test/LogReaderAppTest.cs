using LogReaderApp.Controllers;
using LogReaderApp.Data;
using LogReaderApp.Models;
using LogReaderApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LogReaderApp.Test
{
    public class LogReaderAppTest 
    {
        LogReaderDbContext _context;
        ILogReaderService _service;
        LogsController _controller;      
        
        public LogReaderAppTest()
        {
            _context = new LogReaderDbContext();
            _service = new LogReaderService(_context);
            _controller = new LogsController(_service);
        }

        [Fact, Trait("Priority", "2")]
        public void Get_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetAll().Result as OkObjectResult;
            // Assert
            var items = Assert.IsType<List<Log>>(okResult.Value);
            Assert.NotEmpty(items);
        }

        [Fact, Trait("Priority", "3")]
        public void Add_InvalidObject_ReturnsBadRequest()
        {
            // Arrange
            var testItem = new Log()
            {
                IP = null,
                LogDate = DateTime.Now,
                UserId = 1
            };
            _controller.ModelState.AddModelError("IP", "Required");
            // Act
            var badResponse = _controller.PostLog(testItem);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }
        [Fact, Trait("Priority", "1")]
        public void Add_ValidObject_ReturnsCreatedResponse()
        {
            // Arrange
            var testItem = new Log()
            {
                IP = "200.139.123.784",
                LogDate = DateTime.Now,
                UserId = 1
            };
            // Act
            var createdResponse = _controller.PostLog(testItem);
            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }
        
      

    }
}
