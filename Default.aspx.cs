using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dal;

namespace Root
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Find(txtEmail.Text, txtPassword.Text);

            if (user != null)
            {
                Dal.RolmDBEntities dbcontext = new RolmDBEntities();
                AspNetUser dbuser = dbcontext.AspNetUsers.Where(x => x.Id == user.Id && x.Deleted == null).FirstOrDefault();

                var staffuser = dbcontext.GymStaffs.Where(x => x.UserId == dbuser.Id && x.Deleted == null).FirstOrDefault();

                if (staffuser != null)
                {
                    //Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                    Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                }
                else
                {
                    switch (dbuser.LastAccessedSystemTypeId)
                    {
                        case 1:
                            //Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                           // Response.Redirect("http://216.144.248.226:8096/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            Response.Redirect("http://localhost:1339/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            break;
                        case 2:
                            //Response.Redirect("http://trainer.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                           // Response.Redirect("http://216.144.248.226:8097/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            Response.Redirect("http://localhost:1342/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            break;
                        case 3:
                            //Response.Redirect("http://member.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                             Response.Redirect("http://localhost:1343/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                           // Response.Redirect("http://216.144.248.226:8098/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());

                            break;
                        default:
                            // Response.Redirect("http://gym.rolmfitness.com/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            // Response.Redirect("http://216.144.248.226:8096/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            Response.Redirect("http://localhost:1339/Account/Login?id=" + txtEmail.Text.Trim() + "&password=" + txtPassword.Text.Trim());
                            break;
                    }
                }
                //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                //var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                //if (Request.QueryString["ReturnUrl"] == null)
                //    Response.Redirect("~/Secured/Home.aspx");
            }
            else
            {
                errordiv.Visible = true;
                lblError.Text = "Invalid email or password";
            }
        }
    }
}