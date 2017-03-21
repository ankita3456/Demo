using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dal;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Root
{
    public partial class Register : System.Web.UI.Page
    {
        Dal.RolmDBEntities DbContext;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGyms();
            }
        }

        public Register()
        {
            DbContext = new RolmDBEntities();
        }

        public void LoadGyms()
        {
            var gyms = DbContext.Gyms.Select(x => x).ToList();
            Dal.Gym memsel = new Gym();
            memsel.Id = 0;
            memsel.Title = "Select Gym";
            gyms.Insert(0, memsel);
            ddlGyms.DataSource = gyms;
            ddlGyms.DataTextField = "Title";
            ddlGyms.DataValueField = "Id";
            ddlGyms.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            lblError.Text = string.Empty;
            string checkResult = CheckForErrors();
            string userid = "";
            if (checkResult == "OK")
            {
                // Default UserStore constructor uses the default connection string named: DefaultConnection
                var userStore = new UserStore<IdentityUser>();
                var manager = new UserManager<IdentityUser>(userStore);
                var user = new IdentityUser() { UserName = txtEmailAddress.Text };
                IdentityResult result = manager.Create(user, txtPassword.Text);
                userid = user.Id;
                if (result.Succeeded)
                {
                    DbContext = new RolmDBEntities();
                    AspNetUser Muser = new AspNetUser();
                    Muser = DbContext.AspNetUsers.Where(x => x.UserName == txtEmailAddress.Text && x.Deleted == null).FirstOrDefault();
                    Muser.Email = txtEmailAddress.Text;
                    Muser.EmailConfirmed = true;

                    Muser.Created = DateTime.UtcNow;
                    Muser.Deleted = null;
                    Muser.EmailConfirmed = true;
                    Muser.FullName = txtFirstName.Text + " " + txtLastName.Text;
                    Muser.PhoneNumber = "000000000";
                    Muser.UserName = txtEmailAddress.Text;
                    Muser.GymId = 0;
                    switch (RadioButtonList1.SelectedItem.Value.Trim())
                    {
                        case "Gym Owner":
                            Muser.LastAccessedSystemTypeId = (int)Enum.Parse(typeof(Dal.EnumTypes.SystemTypes), Dal.EnumTypes.SystemTypes.Gym.ToString());
                            break;
                        case "Personal Trainer":
                            Muser.LastAccessedSystemTypeId = (int)Enum.Parse(typeof(Dal.EnumTypes.SystemTypes), Dal.EnumTypes.SystemTypes.Trainer.ToString());
                            Trainer train = new Trainer();
                            train.EmailAddress = txtEmailAddress.Text;
                            train.Gender = RadioButtonList2.SelectedItem.Value.Trim();
                            train.FirstName = txtFirstName.Text;
                            train.LastName = txtLastName.Text;
                            train.UserId = userid;
                            train.Created = DateTime.UtcNow;
                            train.DBImageId = DbContext.DBImages.Where(x => x.Deleted == null).OrderBy(x => x.Id).FirstOrDefault().Id;
                            train.Nationality = 1;
                            train.CountryId = 1;
                            DbContext.Trainers.Add(train);
                            DbContext.SaveChanges();
                            GymTrainer gymTrainer = new GymTrainer();

                            gymTrainer.GymId = Convert.ToInt64(ddlGyms.SelectedValue);
                            gymTrainer.TrainerId = train.Id;
                            gymTrainer.Created = DateTime.UtcNow;
                            DbContext.GymTrainers.Add(gymTrainer);
                            DbContext.SaveChanges();
                            break;
                        case "Fitness Fanatic":

                            Muser.LastAccessedSystemTypeId = (int)Enum.Parse(typeof(Dal.EnumTypes.SystemTypes), Dal.EnumTypes.SystemTypes.Customer.ToString());
                            var Customeruser = DbContext.Customers.Where(x => x.EmailAddress == txtEmailAddress.Text).FirstOrDefault();

                            Customer customer = new Customer();
                            customer.EmailAddress = txtEmailAddress.Text;
                            customer.Gender = RadioButtonList2.SelectedItem.Value.Trim();
                            customer.FirstName = txtFirstName.Text;
                            customer.LastName = txtLastName.Text;
                            customer.UserId = userid;
                            customer.Created = DateTime.UtcNow;
                            customer.DBImageId = DbContext.DBImages.Where(x => x.Deleted == null).OrderBy(x => x.Id).FirstOrDefault().Id;
                            customer.Nationality = 1;
                            customer.CountryId = 1;
                            DbContext.Customers.Add(customer);
                            DbContext.SaveChanges();

                            GymCustomer gymCustomer = new GymCustomer();
                            gymCustomer.GymId = Convert.ToInt64(ddlGyms.SelectedValue);
                            gymCustomer.CustomerId = customer.Id;
                            gymCustomer.Created = DateTime.UtcNow;
                            DbContext.GymCustomers.Add(gymCustomer);
                            DbContext.SaveChanges();

                            break;
                        default:
                            Muser.LastAccessedSystemTypeId = (int)Enum.Parse(typeof(Dal.EnumTypes.SystemTypes), Dal.EnumTypes.SystemTypes.Gym.ToString());
                            break;
                    }


                    DbContext.SaveChanges();

                    switch (RadioButtonList1.SelectedItem.Value.Trim())
                    {
                        case "Gym Owner":
                            //  Response.Redirect("http://gym.rolmfitness.com/Account/Login?id="+ txtEmailAddress.Text.Trim() +"&password="+txtPassword.Text.Trim());
                            //  Response.Redirect("http://216.144.248.226:8096/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());

                            Response.Redirect("http://localhost:1339/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            break;
                        case "Personal Trainer":
                            //   Response.Redirect("http://216.144.248.226:8097/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            //  Response.Redirect("http://216.144.248.226:8097//Schedules/Index?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());

                            // Response.Redirect("http://trainer.rolmfitness.com/Account/Login?id="+ txtEmailAddress.Text.Trim() +"&password="+txtPassword.Text.Trim());
                            Response.Redirect("http://localhost:1342/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            break;
                        case "Fitness Fanatic":
                            //  Response.Redirect("http://216.144.248.226:8098/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            //Response.Redirect("http://member.rolmfitness.com/Account/Login?id="+ txtEmailAddress.Text.Trim() +"&password="+txtPassword.Text.Trim());
                            Response.Redirect("http://localhost:1343/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());

                            break;
                        default:
                            //  Response.Redirect("http://216.144.248.226:8096/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            //Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            Response.Redirect("http://localhost:1339/Account/Login?id=" + txtEmailAddress.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            break;
                    }
                }
                else
                {
                    lblError1.Text = result.Errors.FirstOrDefault();

                }





            }
            else
            {
                lblError1.Text = checkResult;
            }

        }

        public string CheckForErrors()
        {
            string result = "";



            if (DbContext.AspNetUsers.Where(x => x.Email == txtEmailAddress.Text && x.Deleted == null).Count() != 0)
            {
                result += "Email already exist !";
            }
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                result += Environment.NewLine + "Confirm Pasword not matched !";
            }

            //if(Regex.Match(txtPassword.Text, @"/\d+/", RegexOptions.None).Success)
            //{
            //     result +=  Environment.NewLine + "Password must contain atleast three numbers !";
            //}

            //if(Regex.Match(txtPassword.Text, @"/[a-z]/", RegexOptions.None).Success)
            //{
            //     result +=  Environment.NewLine + "Password must contain atleast three lower case characters !";
            //}

            //if(Regex.Match(txtPassword.Text, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
            //{
            //     result +=  Environment.NewLine + "Password must contain atleast three upper case characters !";
            //}

            //if(Regex.Match(txtPassword.Text, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
            //{
            //     result +=  Environment.NewLine + "Password must contain atleast one special character !";
            //}

            if (string.IsNullOrEmpty(result))
            {
                result = "OK";
            }


            return result;
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (RadioButtonList1.SelectedItem.Value.Trim())
            {
                case "Gym Owner":
                    divddlgym.Visible = false;
                    divinfo.Visible = true;
                    break;
                case "Personal Trainer":
                    divddlgym.Visible = true;
                    divinfo.Visible = true;
                    break;
                case "Fitness Fanatic":
                    divddlgym.Visible = true;
                    divinfo.Visible = true;
                    break;
                default:

                    break;
            }
        }

        protected void btnLoginSubmit_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Find(txtEmail.Text, txtPasswordlogin.Text);

            if (user != null)
            {
                Dal.RolmDBEntities dbcontext = new RolmDBEntities();
                AspNetUser dbuser = dbcontext.AspNetUsers.Where(x => x.Id == user.Id && x.Deleted == null).FirstOrDefault();

                var staffuser = dbcontext.GymStaffs.Where(x => x.UserId == dbuser.Id && x.Deleted == null).FirstOrDefault();

                if (staffuser != null)
                {
                    //Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                    //Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                    //  Response.Redirect("http://216.144.248.226:8096/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                    Response.Redirect("http://localhost:1339/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                }
                else
                {
                    switch (dbuser.LastAccessedSystemTypeId)
                    {
                        case 1:
                            //Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            //   Response.Redirect("http://216.144.248.226:8096/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            Response.Redirect("http://localhost:1339/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            break;
                        case 2:
                            //Response.Redirect("http://trainer.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            //   Response.Redirect("http://216.144.248.226:8097/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            //  Response.Redirect("http://216.144.248.226:8097/Schedules/Index?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            Response.Redirect("http://localhost:1342/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            break;
                        case 3:
                            //Response.Redirect("http://member.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            Response.Redirect("http://localhost:1343/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            //   Response.Redirect("http://216.144.248.226:8098/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());

                            break;
                        default:
                            // Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            // Response.Redirect("http://216.144.248.226:8096/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            Response.Redirect("http://localhost:1339/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPasswordlogin.Text.Trim());
                            break;
                    }
                }
            }
            else
            {
                // errordiv.Visible = true;
                lblError.Text = "Invalid email or password";
            }
        }
    }
}