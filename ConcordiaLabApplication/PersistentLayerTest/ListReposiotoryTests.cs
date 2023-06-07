using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayerTest
{
    public class ListReposiotoryTests
    {
        private readonly ListRepository _sut;

        public ListReposiotoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseSqlServer("Data Source=DESKTOP-476F63V\\SQLEXPRESS;Initial Catalog=ConcordiaLab;Integrated Security=true;TrustServerCertificate=True;")
                .Options;

            var _dbContext = new ConcordiaDbContext(dbContextOptions);
            _sut = new ListRepository(_dbContext);
        }

        [Fact]
        public void Should_Return_All_Lists()
        {
            var result = _sut.GetAll();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Should_Return_Lists_By_ListId()
        {
            var result = _sut.GetById(1);
            Assert.Equal("to do", result.Title);
            var result2 = _sut.GetById(2);
            Assert.Equal("in progress", result2.Title);
            var result3 = _sut.GetById(3);
            Assert.Equal("done", result3.Title);
        }

        [Fact]
        public void Shoul_Return_Lists_Of_A_Scientist_By_ScientisId()
        {
            var result = _sut.GetByScientistId(1); // questo scienziato ha due liste
            Assert.Equal(2, result.Count());
            foreach (var list in result)
            {
                var l = list;
                Assert.NotNull(list.Experiments);
                foreach (var experiment in list.Experiments)
                {
                    var e = experiment;
                    Assert.NotNull(experiment.Scientists);
                }
            }
        }
    }
}
