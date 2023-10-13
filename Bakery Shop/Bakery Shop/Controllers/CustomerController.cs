using Bakery_Shop.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Bakery_Shop.Controllers
{
    public class CustomerController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Customer
        public ActionResult PlaceOrder()
        {
            return View();
        }

        public ActionResult Orders()
        {
            var UserId = User.Identity.GetUserId();
            var UserOrders = context.Orders.Where(x=>x.CustomerId == UserId).ToList();
            ViewBag.Count = context.Orders.Where(x => x.CustomerId == UserId).Count();

            if (UserOrders != null)
            {
                return View(UserOrders);
            }
            return View();
        }

        public ActionResult OrderDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var Order = context.Orders.Find(id);
            return View(Order);
        }

        public ActionResult PayLater(string OrderNo)
        {
            var Order = context.Orders.FirstOrDefault(x => x.OrderNo == OrderNo);
            if (Order != null)
            {
                return RedirectToAction("MakePayment", "ShoppingCart");
            }
            else
                return View("OrderError");
        }

        public ActionResult OrderError()
        {
            return View();
        }

        public ActionResult ConfirmDelivery(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = context.Orders.Find(id);
            if (order != null)
            {
                string Body = "Dear " + order.FirstName + "</br>" +
                    "Your order has been delivered </br>" +
                    "</br>" +
                    "Regards" + "<br/>" +
                    "Royal Bakery";

                order.IsDeliveryConfirmed = true;
                context.SaveChanges();


                //Send SMS to the registered Number
                var accounySid = ConfigurationManager.AppSettings["AccountSid"];
                var authToken = ConfigurationManager.AppSettings["AuthToken"];

                TwilioClient.Init(accounySid, authToken);
                string Name = order.FirstName;
                //string Title = booking.Title;
                //string BookingKey = Booking.BookingKey;
                //string OTP = Booking.OneTimePin;
                //string Email = booking.Èmail;
                //string PhoneNo = order.CustomerPhone;
                //var DriverId = order.AssignedDriverId;
                //Driver driver = context.Drivers.Find(DriverId);

                //var To = new PhoneNumber("+27" + PhoneNo);
                //var From = new PhoneNumber("+18305875189");

                //var Message = MessageResource.Create(
                //    to: To,
                //    from: From,
                //    body: "\n\nHi " + Name + "," +
                //    "\nyour order has been processed, an email with your receipt has been sent to "
                //    + order.CustomerEmail +
                //    "\nYour order was delivered on " + order.OrderDate + "\n" +

                //    "By " + driver.Name +

                //    "\nThank you for choosing us" +

                //    "\n\nYour sincerely\nRoyal Bakery"
                //    );

                var OrderNo = order.OrderId;
                var TrackingNumber = order.TrackingNumber;
                var OrderDetails = order.OrderDetails;
                var TotalAmount = order.Total;


                Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                //Top Heading
                Chunk chunk = new Chunk("Order #" + OrderNo + " Receipt.");
                pdfDoc.Add(chunk);

                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                pdfDoc.Add(line);


                //Table
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                //0=Left, 1=Centre, 2=Right
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell no 1
                PdfPCell cell = new PdfPCell();
                cell.Border = 0;
                Image image = Image.GetInstance(Server.MapPath("~/Content/Images/logo.png"));
                image.ScaleAbsolute(150, 100);
                cell.AddElement(image);
                table.AddCell(cell);

                //Cell no 2
                chunk = new Chunk("Order No : #" + OrderNo);
                cell = new PdfPCell();
                cell.Border = 0;
                cell.AddElement(chunk);
                table.AddCell(cell);

                //Add table to document
                pdfDoc.Add(table);


                //Table
                table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;

                table.SpacingBefore = 30f;
                table.SpacingAfter = 30f;
                table.DefaultCell.Padding = 4;

                //Cell
                cell = new PdfPCell(new Phrase("Order Details"));
                cell.Colspan = 3;
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 8f;

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);

                table.AddCell(new PdfPCell(new Phrase("Product")));
                table.AddCell(new PdfPCell(new Phrase("Qty")));
                table.AddCell(new PdfPCell(new Phrase("Price")));

                foreach (var item in OrderDetails)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Product.ProductName)));
                    table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString())));
                    table.AddCell(new PdfPCell(new Phrase(item.ProductPrice.ToString())));
                }

                //line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                //pdfDoc.Add(line);


                cell = new PdfPCell(new Phrase("Amount Due : R" + TotalAmount));
                cell.Colspan = 3;
                cell.PaddingTop = 10f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.PaddingBottom = 10f;
                cell.PaddingRight = 30f;
                table.AddCell(cell);

                pdfDoc.Add(table);

                Paragraph para = new Paragraph();
                para.Add("Hello " + Name + ",\n\nThank you for choosing Royal Bakery");

                pdfDoc.Add(para);

                //pdfWriter.CloseStream = false;
                //pdfDoc.Close();

                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                memoryStream.Position = 0;
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=Receipt.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.Write(pdfDoc);
                //Response.End();

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
                msg.To.Add(new MailAddress(order.CustomerEmail));
                msg.Subject = "Successful Delivery";
                msg.Body = Body;
                msg.IsBodyHtml = true;
                msg.Attachments.Add(new Attachment(memoryStream, "OrderReport.pdf"));
                SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
                smtp.Timeout = 1000000;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential nc = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["Email"].ToString(), System.Configuration.ConfigurationManager.AppSettings["Password"].ToString());
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = nc;
                smtp.Send(msg);
                msg.Dispose();
            }
            return View("DeliveryConfirmed");
        }

        public ActionResult DeliveryConfirmed()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Cancelorder(Ordercancelation model,int ordid)
        {

            model.OrderId = ordid;

            return View(model);
        }


        [HttpPost]
        public ActionResult Cancelorder(Ordercancelation model)
        {

            Ordercancelation oc = new Ordercancelation()
            {
                Date = DateTime.Now,
                OrderId = model.OrderId,
                Reason = model.Reason,
                Status = "Order cancelation submited",
                Statusnum = 1,

            };

            context.Ordercancelations.Add(oc);
            context.SaveChanges();



            Order o = context.Orders.Find(model.OrderId);
            o.Status = "Order cancellation submited";
            context.SaveChanges();

            return RedirectToAction("Orders");
        }


    }
}