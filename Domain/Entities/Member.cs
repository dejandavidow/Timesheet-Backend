using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Member
    {
        public Member(Guid id,string Name,string Username,string Email,float Hours,string Status,string Role,string Password)
        {
            this.Id=id;
            this.Name=Name;
            this.Username=Username;
            this.Email=Email;
            this.Hours=Hours;
            this.Status=Status;
            this.Role=Role;
            this.Password=Password;
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Username{get; private set;}
        public string Email { get; private set; }
        public float Hours { get; private set; }
        public string Status { get; private set; }
        public string Role { get; private set; }
        public string Password { get; private set; }
        public Member UpdateName(string name)
        {
            return new Member(this.Id,name ?? this.Name,this.Username,this.Email,this.Hours,this.Status,this.Role,this.Password);
        }
        public Member UpdateUserName(string username)
        {
            return new Member(this.Id,this.Name,username ?? this.Username,this.Email,this.Hours,this.Status,this.Role, this.Password);
        } 
        public Member UpdateEmail(string email)
        {
            return new Member(this.Id,this.Name,this.Username,email ?? this.Email,this.Hours,this.Status,this.Role, this.Password);
        }
        public Member UpdateHours(float hours)
        {
            return new Member(this.Id,this.Name,this.Username,this.Email, hours >0 ? hours : this.Hours , this.Status,this.Role, this.Password);
        }
        public Member UpdateStatus(string status)
        {
            return new Member(this.Id, this.Name, this.Username, this.Email, this.Hours,status ?? this.Status,this.Role, this.Password);
        } 
        public Member UpdateRole(string role)
        {
            return new Member(this.Id,this.Name,this.Username,this.Email,this.Hours,this.Status, role ?? this.Role, this.Password);
        }
        public Member UpdatePassword(string password)
        {
            return new Member(this.Id, this.Name, this.Username, this.Email, this.Hours, this.Status, this.Role, password ?? this.Password);
        }
    }
}
