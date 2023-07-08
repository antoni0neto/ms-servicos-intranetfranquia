using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ImagemController
    {
        DCDataContext db;

        public IMAGEM_PRODUTO BuscaPorCodigoImagemProduto(int CODIGO_IMAGEM)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.IMAGEM_PRODUTOs where i.CODIGO_IMAGEM_PRODUTO == CODIGO_IMAGEM select i).SingleOrDefault();
        }

        public IMAGEM BuscaPorCodigoImagem(int CODIGO_IMAGEM)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.IMAGEMs where i.CODIGO_IMAGEM == CODIGO_IMAGEM select i).SingleOrDefault();
        }

        public List<IMAGEM> BuscaImagens(int CODIGO_LOJA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.IMAGEMs where i.CODIGO_LOJA == CODIGO_LOJA where i.ATIVO == true orderby i.NOME_IMAGEM select i).ToList();
        }

        public List<IMAGEM> BuscaImagensDeposito(int CODIGO_LOJA, DateTime DATA_DIGITADA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.IMAGEMs where i.CODIGO_LOJA == CODIGO_LOJA & i.DATA_DIGITADA == DATA_DIGITADA orderby i.NOME_IMAGEM select i).ToList();
        }

        public List<IMAGEM_PRODUTO> BuscaImagensProduto()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from ip in db.IMAGEM_PRODUTOs where ip.ATIVO == true orderby ip.CODIGO_PRODUTO select ip).ToList();
        }

        public void InsereImagemProduto(IMAGEM_PRODUTO imagem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMAGEM_PRODUTOs.InsertOnSubmit(imagem);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insere(IMAGEM imagem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMAGEMs.InsertOnSubmit(imagem);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Atualiza(IMAGEM imagemNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMAGEM imagemOld = BuscaPorCodigoImagem(imagemNew.CODIGO_IMAGEM);
            if (imagemOld != null)
            {
                imagemOld.NOME_IMAGEM = imagemNew.NOME_IMAGEM;
                imagemOld.LOCAL_IMAGEM = imagemNew.LOCAL_IMAGEM;
                imagemOld.ATIVO = imagemNew.ATIVO;
                imagemOld.CODIGO_LOJA = imagemNew.CODIGO_LOJA;
            }

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirProduto(int CODIGO_IMAGEM)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMAGEM_PRODUTO imagem = BuscaPorCodigoImagemProduto(CODIGO_IMAGEM);

                if (imagem != null)
                {
                    db.IMAGEM_PRODUTOs.DeleteOnSubmit(imagem);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Exclui(int CODIGO_IMAGEM)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMAGEM imagem = BuscaPorCodigoImagem(CODIGO_IMAGEM);
                if (imagem != null)
                {
                    db.IMAGEMs.DeleteOnSubmit(imagem);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereImagemVinculoDepositoDocumento(IMAGEM_VINCULO_DEPOSITO_DOCUMENTO imagem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMAGEM_VINCULO_DEPOSITO_DOCUMENTOs.InsertOnSubmit(imagem);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiImagemVinculoDepositoDocumento(int CODIGO_IMAGEM, int CODIGO_DOCUMENTO, int CODIGO_DEPOSITO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMAGEM_VINCULO_DEPOSITO_DOCUMENTO imagem = new IMAGEM_VINCULO_DEPOSITO_DOCUMENTO();
                imagem = BuscaImagemVinculoDepositoDocumento(CODIGO_IMAGEM, CODIGO_DOCUMENTO, CODIGO_DEPOSITO);

                if (imagem != null)
                {
                    db.IMAGEM_VINCULO_DEPOSITO_DOCUMENTOs.DeleteOnSubmit(imagem);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IMAGEM_VINCULO_DEPOSITO_DOCUMENTO BuscaImagemVinculoDepositoDocumento(int CODIGO_IMAGEM, int CODIGO_DOCUMENTO, int CODIGO_DEPOSITO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.IMAGEM_VINCULO_DEPOSITO_DOCUMENTOs where (i.CODIGO_IMAGEM == CODIGO_IMAGEM | CODIGO_IMAGEM == 0) & (i.CODIGO_DOCUMENTO == CODIGO_DOCUMENTO | CODIGO_DOCUMENTO == 0) & (i.CODIGO_DEPOSITO == CODIGO_DEPOSITO | CODIGO_DEPOSITO == 0) select i).Take(1).SingleOrDefault();
        }
    }
}
