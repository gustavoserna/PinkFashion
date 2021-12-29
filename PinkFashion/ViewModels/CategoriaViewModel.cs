﻿using Newtonsoft.Json;
using PinkFashion.Models;
using PinkFashion.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PinkFashion.ViewModels
{
    class CategoriaViewModel : InsigniaViewModel
    {
        public INavigation Navigation { get; set; }
        Categoria_ categoria;

        bool boolSubrayaSubcategoria = false;

        string idCategoria = "";
        string idSubcategoria = "";
        string idMarca = "";
        string filtroPrecio = "";
        json_object json_ob = new json_object();
        public ObservableCollection<ColeccionSubcategorias> ColSubCategorias { get; set; }
        public ObservableCollection<Producto_> Productos { get; set; }

        public Command LoadProductosCommand { get; set; }
        public Command LoadSubcategoriasCommand { get; set; }

        public string _NameCategoria;
        
        public string NameCategoria
        {
            get { return _NameCategoria; }
            set { SetProperty(ref _NameCategoria, value); }
        }

        bool _ListViewVisible = true;
        public bool ListViewVisible
        {
            get { return _ListViewVisible; }
            set { SetProperty(ref _ListViewVisible, value); }
        }

        bool _NoEncontradoVisible = false;
        public bool NoEncontradoVisible
        {
            get { return _NoEncontradoVisible; }
            set { SetProperty(ref _NoEncontradoVisible, value); }
        }

        public CategoriaViewModel(Categoria_ categoria, INavigation navigation)
        {
            this.categoria = categoria;
            this.Navigation = navigation;
            Productos = new ObservableCollection<Producto_>();
            ColSubCategorias = new ObservableCollection<ColeccionSubcategorias>();
            NameCategoria = categoria.Categoria;

            LoadProductosCommand = new Command(async () =>
            {
                await ExecuteLoadProductosCommand();
            });

            LoadSubcategoriasCommand = new Command(async () =>
            {
                await ExecuteLoadSubcategoriasCommand();
            });
        }

        async Task ExecuteLoadProductosCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Productos.Clear();
                IEnumerable<Producto_> productos = null;
                List<Producto_> lista = new List<Producto_>();
                await GetProductos().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        for (int i = 0; i < t.Result.Length; i++)
                        {
                            lista.Add(t.Result[i]);
                        }
                    }
                });
                
                productos = lista;

                foreach (var item in productos)
                {
                    Productos.Add(item);
                }

                if (Productos.Count == 0)
                {
                    ListViewVisible = false;
                    NoEncontradoVisible = true;
                }
                else
                {
                    ListViewVisible = true;
                    NoEncontradoVisible = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        
        async Task ExecuteLoadSubcategoriasCommand()
        {
            try
            {
                ColSubCategorias.Clear();
                List<Subcategoria_> listasubcategorias_for_col = new List<Subcategoria_>();

                List<Subcategoria_> listasubcategorias = new List<Subcategoria_>();

                await GetSubcategorias().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        foreach (Subcategoria_ subcategoria in t.Result)
                        {
                            //Vease SubcategoriaTappedCommand
                            if (String.IsNullOrEmpty(idSubcategoria))
                            {
                                subcategoria.IsUnderlined = false;
                                listasubcategorias.Add(subcategoria);
                                listasubcategorias_for_col.Add(subcategoria);
                            } else
                            {
                                if(subcategoria.idsubcategorias.Equals(idSubcategoria))
                                {
                                    subcategoria.IsUnderlined = true;
                                    listasubcategorias.Insert(0, subcategoria);
                                    listasubcategorias_for_col.Insert(0, subcategoria);
                                    continue;
                                }
                                subcategoria.IsUnderlined = false;
                                listasubcategorias.Add(subcategoria);
                                listasubcategorias_for_col.Add(subcategoria);
                            }
                        }
                    }
                });

                //carousel subcategorias
                double totalSlidesSubcategorias = Math.Ceiling((Double)listasubcategorias_for_col.Count / 3);
                for (int i = 0; i < totalSlidesSubcategorias; i++)
                {
                    ColeccionSubcategorias coleccion = new ColeccionSubcategorias();
                    coleccion.subcategorias = new List<Subcategoria_>();

                    for (int k = 0; k <= listasubcategorias_for_col.Count; k++)
                    {
                        if (k < 3 && listasubcategorias_for_col.Count > 0)
                        {
                            coleccion.subcategorias.Add(listasubcategorias_for_col[0]);
                            listasubcategorias_for_col.RemoveAt(0);
                        }
                        else
                        {
                            break;
                        }
                    }

                    ColSubCategorias.Add(coleccion);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public ICommand ProductoTappedCommand
        {
            get
            {
                return new Command<Producto_>(async (Producto_ model) =>
                {
                    await Navigation.PushAsync(new Producto(model));
                });
            }
        }

        public ICommand RemoverFiltrosCommand
        {
            get
            {
                return new Command(() =>
                {
                    this.idMarca = "";
                    this.filtroPrecio = "";
                    this.idSubcategoria = "";
                    LoadProductosCommand.Execute(null);
                });
            }
        }

        public ICommand FiltroPrecioCommand
        {
            get
            {
                return new Command(() =>
                {
                    this.filtroPrecio = "menorMayor";
                    LoadProductosCommand.Execute(null);
                });
            }
        }

        public ICommand AbrirMarcasCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Subscribe<MarcasViewModel, string>(this, "idMarca", (sender, arg) =>
                    {
                        this.idMarca = arg;
                        LoadProductosCommand.Execute(null);
                    });

                    var page = new NavigationPage(new MarcasVista(this.categoria));
                    page.BarBackgroundColor = App.bgColor;
                    page.BarTextColor = App.textColor;
                    App.Current.MainPage.Navigation.PushModalAsync(page);
                });
            }
        }

        public ICommand SubcategoriaTappedCommand
        {
            get
            {
                return new Command<Subcategoria_>(async (Subcategoria_ subcategoria) =>
                {
                    //Vuevlo a ejecutar LoadSubCategorias para volver a hacer el binding
                    //Y esta vez aplicar el subrayado a idSubcategoria
                    boolSubrayaSubcategoria = true;
                    idSubcategoria = subcategoria.idsubcategorias;
                    
                    LoadSubcategoriasCommand.Execute(null);
                    LoadProductosCommand.Execute(null);
                });
            }
        }

        public async Task<Producto_[]> GetProductos()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = null;
                if (!idSubcategoria.Equals(""))
                {
                    if (!idMarca.Equals("") && filtroPrecio.Equals(""))
                    {
                        str = new StringContent("op=ObtenerProductosSubCategoria&idsubcategoria=" + this.idSubcategoria + "&idMarca=" + idMarca, Encoding.UTF8, "application/x-www-form-urlencoded");
                    }
                    else if(!filtroPrecio.Equals("") && idMarca.Equals(""))
                    {
                        str = new StringContent("op=ObtenerProductosSubCategoria&idsubcategoria=" + this.idSubcategoria + "&filtroPrecio=" + filtroPrecio, Encoding.UTF8, "application/x-www-form-urlencoded");
                    }
                    else if(!filtroPrecio.Equals("") && !idMarca.Equals(""))
                    {
                        str = new StringContent("op=ObtenerProductosSubCategoria&idsubcategoria=" + this.idSubcategoria + "&idMarca=" + idMarca + "&filtroPrecio=" + filtroPrecio, Encoding.UTF8, "application/x-www-form-urlencoded");
                    }
                    else
                    {
                        str = new StringContent("op=ObtenerProductosSubCategoria&idcategoria=" + this.categoria.IdCategoria + "&idsubcategoria=" + this.idSubcategoria, Encoding.UTF8, "application/x-www-form-urlencoded");
                    }
                }
                else if (!idMarca.Equals("") && filtroPrecio.Equals(""))
                {
                    str = new StringContent("op=ObtenerProductosCategoria&idcategoria=" + this.categoria.IdCategoria + "&idMarca=" + idMarca, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else if (!filtroPrecio.Equals("") && idMarca.Equals(""))
                {
                    str = new StringContent("op=ObtenerProductosCategoria&idcategoria=" + this.categoria.IdCategoria + "&filtroPrecio=" + filtroPrecio, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else if (!filtroPrecio.Equals("") && !idMarca.Equals(""))
                {
                    str = new StringContent("op=ObtenerProductosCategoria&idcategoria=" + this.categoria.IdCategoria + "&idMarca=" + idMarca + "&filtroPrecio=" + filtroPrecio, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else
                {
                    str = new StringContent("op=ObtenerProductosCategoria&idcategoria=" + this.categoria.IdCategoria, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                var respuesta = await client.PostAsync(Constantes.url + "Productos/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Productos: " + json);


                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.productos = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.productos;

        }

        public async Task<Subcategoria_[]> GetSubcategorias()
        {
            try
            {
                var client = new HttpClient();
                StringContent str = new StringContent("op=subcategorias&idCategoria=" + this.categoria.IdCategoria, Encoding.UTF8, "application/x-www-form-urlencoded");
                var respuesta = await client.PostAsync(Constantes.url + "Listas/App.php", str);
                var json = respuesta.Content.ReadAsStringAsync().Result.Trim();
                System.Diagnostics.Debug.WriteLine("Subcategorias: " + json);


                if (json != "")
                {
                    json_ob = JsonConvert.DeserializeObject<json_object>(json);
                }
                else
                {
                    return json_ob.subcategorias = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return json_ob.subcategorias;
        }
    }

    public class json_object
    {
        [JsonProperty("ListadoProductos")]
        public Producto_[] productos { get; set; }

        [JsonProperty("EntidadSubcategorias")]
        public Subcategoria_[] subcategorias { get; set; }

    }
}
