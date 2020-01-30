using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FreelanceDir.Data;
using FreelanceDir.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FreelanceDir.Pages.Messages
{
    public class IndexModel : PageModel
    {
        private readonly FreelanceDir.Data.RDSContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(FreelanceDir.Data.RDSContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
                
        public ILookup<string, Message> Messages { get; set; }     
        [BindProperty]
        public Message Message { get; set; }

        private ILookup<string, Message> GetMessages()
        {
            return _context.Messages
                .Where(m => m.SenderId == _userManager.GetUserId(User) || m.ReceiverId == _userManager.GetUserId(User))
                .ToLookup(m => m.ReceiverId != _userManager.GetUserId(User) ? _context.Users.Find(m.ReceiverId).UserName : _context.Users.Find(m.SenderId).UserName, m => m);
        }

        public void OnGet()
        {
            Messages = GetMessages();
        }

        public IActionResult OnGetUserConvo(string otherusername)
        {
            OnGet();
            return new PartialViewResult
            {
                ViewName = "_ConvoPartial",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = Messages[otherusername].ToList(),
                },
            };
        }

        public async Task<IActionResult> OnPostSendAsync(string receivername)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message.Receiver = await _userManager.FindByNameAsync(receivername);
            Message.Sender = await _userManager.GetUserAsync(User);

            _context.Messages.Add(Message);
            await _context.SaveChangesAsync();

            return OnGetUserConvo(receivername);
        }
    }
}
