using System;
using System.Collections.Generic;
using ImageConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageConverter_UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMakeFileSizeToReadableString()
        {
            var testMainWindowPresenter = new MainWindowPresenter();
            double length = 1000;
            string expected = "1000 B";
            Assert.AreEqual(expected , testMainWindowPresenter.MakeFileSizeToReadableString(length));
            length = 1024;
            expected = "1 KB";
            Assert.AreEqual(expected, testMainWindowPresenter.MakeFileSizeToReadableString(length));
        }

        [TestMethod]
        public void TestIsFileCompressable()
        {
            var testMainWindowPresenter = new MainWindowPresenter();

            string check = "Tesssttt.jpg";
            bool expected = true;
            Assert.AreEqual(expected, testMainWindowPresenter.IsFileCompressable(check));
            check = ".jpg";
            expected = true;
            Assert.AreEqual(expected, testMainWindowPresenter.IsFileCompressable(check));
            check = ".sys";
            expected = false;
            Assert.AreEqual(expected, testMainWindowPresenter.IsFileCompressable(check));
            check = "someUnknownFile";
            expected = false;
            Assert.AreEqual(expected, testMainWindowPresenter.IsFileCompressable(check));
            check = "";
            expected = false;
            Assert.AreEqual(expected, testMainWindowPresenter.IsFileCompressable(check));


        }


    }
}
