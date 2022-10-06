// See https://aka.ms/new-console-template for more information

using CommandRelay;

var messageService = new MessageService();
messageService.Send(string.Join(",", args));


