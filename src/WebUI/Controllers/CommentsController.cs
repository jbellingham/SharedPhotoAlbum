﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedPhotoAlbum.Application.Comments.Commands.CreateComment;

namespace SharedPhotoAlbum.WebUI.Controllers
{
    public class CommentsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateCommentCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}