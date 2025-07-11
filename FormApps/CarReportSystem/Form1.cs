using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using static CarReportSystem.CarReport;

namespace CarReportSystem {
    public partial class Form1 : Form {
        //�J�[���|�[�g�Ǘ��p���X�g
        BindingList<CarReport> listCarReports = new BindingList<CarReport>();

        //�ݒ�N���X�̃C���X�^���X�𐶐�
        Settings settings = new Settings();

        public Form1() {
            InitializeComponent();
            dgvRecord.DataSource = listCarReports;
        }

        private void btPicOpen_Click(object sender, EventArgs e) {
            if (ofdPicFileOpen.ShowDialog() == DialogResult.OK) {
                pbPicture.Image = Image.FromFile(ofdPicFileOpen.FileName);
            }
        }

        private void btPicDelete_Click(object sender, EventArgs e) {
            pbPicture.Image = null;
        }

        //�L�^�҂̗������R���{�{�b�N�X�֓o�^�i�d���Ȃ��j
        private void setCbAuthor(string author) {
            if (!cbAuthor.Items.Contains(author)) {
                //���o�^�Ȃ�o�^�y�o�^�ς݂Ȃ牽�����Ȃ��z
                cbAuthor.Items.Add(author);
            }
        }

        //�Ԗ��̗������R���{�{�b�N�X�֓o�^�i�d���Ȃ��j
        private void setCbCarName(string carName) {
            if (!cbCarName.Items.Contains(carName)) {
                //���o�^�Ȃ�o�^�y�o�^�ς݂Ȃ牽�����Ȃ��z
                cbCarName.Items.Add(carName);
            }
        }

        private void btRecordAdd_Click(object sender, EventArgs e) {

            tsslbMessage.Text = String.Empty;

            if (cbAuthor.Text == String.Empty || cbCarName.Text == String.Empty) {
                tsslbMessage.Text = "�L�^�ҁA�܂��͎Ԗ��������͂ł�";
                return;
            }

            var carReport = new CarReport {
                Date = dtpDate.Value,
                Author = cbAuthor.Text,
                Maker = getRadioButtonMaker(),
                CarName = cbCarName.Text,
                Report = tbReport.Text,
                Picture = pbPicture.Image,
            };
            listCarReports.Add(carReport);
            setCbAuthor(cbAuthor.Text); //�R���{�{�b�N�X�֓o�^
            setCbCarName(cbCarName.Text);
            InputItemsAllClear();   //�o�^��͍��ڂ��N���A
        }

        //���͍��ڂ����ׂăN���A
        private void InputItemsAllClear() {
            dtpDate.Value = DateTime.Today;
            cbAuthor.Text = string.Empty;
            rbOther.Checked = true;
            cbCarName.Text = string.Empty;
            tbReport.Text = string.Empty;
            pbPicture.Image = null;
        }

        private MakerGroup getRadioButtonMaker() {
            if (rbToyota.Checked)
                return MakerGroup.�g���^;
            if (rbNissan.Checked)
                return MakerGroup.���Y;
            if (rbHonda.Checked)
                return MakerGroup.�z���_;
            if (rbSubaru.Checked)
                return MakerGroup.�X�o��;
            if (rbImport.Checked)
                return MakerGroup.�A����;

            return MakerGroup.���̑�;
        }

        private void dgvRecord_Click(object sender, EventArgs e) {
            if (dgvRecord.CurrentRow is null) return;

            dtpDate.Value = (DateTime)dgvRecord.CurrentRow.Cells["Date"].Value;
            cbAuthor.Text = (string)dgvRecord.CurrentRow.Cells["Author"].Value;
            setRadioButtonMaker((MakerGroup)dgvRecord.CurrentRow.Cells["Maker"].Value);
            cbCarName.Text = (string)dgvRecord.CurrentRow.Cells["CarName"].Value;
            tbReport.Text = (string)dgvRecord.CurrentRow.Cells["Report"].Value;
            pbPicture.Image = (Image)dgvRecord.CurrentRow.Cells["Picture"].Value;
        }

        //�w�肵�����[�J�[�̃��W�I�{�^�����Z�b�g
        private void setRadioButtonMaker(MakerGroup targetMaker) {
            switch (targetMaker) {
                case MakerGroup.�g���^:
                    rbToyota.Checked = true;
                    break;
                case MakerGroup.���Y:
                    rbNissan.Checked = true;
                    break;
                case MakerGroup.�z���_:
                    rbHonda.Checked = true;
                    break;
                case MakerGroup.�X�o��:
                    rbSubaru.Checked = true;
                    break;
                case MakerGroup.�A����:
                    rbImport.Checked = true;
                    break;
                default:
                    rbOther.Checked = true;
                    break;
            }
        }
        //�V�K�ǉ��̃C�x���g�n���h��
        private void btNewRecord_Click(object sender, EventArgs e) {
            InputItemsAllClear();
        }

        //�C���{�^���̃C�x���g�n���h��
        private void btRecordModify_Click(object sender, EventArgs e) {
            //if ((dgvRecord.CurrentRow is null)
            //    || (!dgvRecord.CurrentRow.Selected)) return;

            if (dgvRecord.Rows.Count == 0) return;

            listCarReports[dgvRecord.CurrentRow.Index].Date = dtpDate.Value;
            listCarReports[dgvRecord.CurrentRow.Index].Author = cbAuthor.Text;
            listCarReports[dgvRecord.CurrentRow.Index].Maker = getRadioButtonMaker();
            listCarReports[dgvRecord.CurrentRow.Index].CarName = cbCarName.Text;
            listCarReports[dgvRecord.CurrentRow.Index].Report = tbReport.Text;
            listCarReports[dgvRecord.CurrentRow.Index].Picture = pbPicture.Image;

            dgvRecord.Refresh();    //�f�[�^�O���b�h�r���[�̍X�V
        }

        //�폜�{�^���̃C�x���g�n���h��
        private void btRecordDelete_Click(object sender, EventArgs e) {
            //�I������Ă��Ȃ��ꍇ�͏������s��Ȃ�
            if ((dgvRecord.CurrentRow is null)
                || (!dgvRecord.CurrentRow.Selected)) return;

            //�I������Ă���C���f�b�N�X���擾
            int index = dgvRecord.CurrentRow.Index;
            //�폜�������C���f�b�N�X���w�肵�ă��X�g����폜
            listCarReports.RemoveAt(index);
        }

        private void Form1_Load(object sender, EventArgs e) {
            InputItemsAllClear();

            //���݂ɐF��ݒ�i�f�[�^�O���b�h�r���[�j
            dgvRecord.DefaultCellStyle.BackColor = Color.LightGray;
            dgvRecord.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;

            //�ݒ�t�@�C����ǂݍ��ݔw�i�F��ݒ肷��i�t�V���A�����j
            //P286�ȍ~���Q�l�ɂ���i�t�@�C�����Fsetting.xml�j



        }

        private void tsmiExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        //���̃A�v���ɂ��Ă�I���������̃C�x���g�n���h��
        private void tsmiAbout_Click(object sender, EventArgs e) {
            fmVersion fmv = new fmVersion();
            fmv.ShowDialog();
        }

        private void �F�ݒ�ToolStripMenuItem_Click(object sender, EventArgs e) {

            if (cdColor.ShowDialog() == DialogResult.OK) {
                BackColor = cdColor.Color;
                //�ݒ�t�@�C���֕ۑ�
                settings.MainFormBackColor = cdColor.Color.ToArgb();    //�w�i�F��ݒ�C���X�^���X�֐ݒ�

            }
        }

        //�t�@�C���I�[�v������
        private void reportOpenFile() {
            if (ofdReportFileOpen.ShowDialog() == DialogResult.OK) {
                try {
                    //�t�V���A�����Ńo�C�i���`������荞��
#pragma warning disable SYSLIB0011 // �^�܂��̓����o�[�����^���ł�
                    var bf = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // �^�܂��̓����o�[�����^���ł�
                    using (FileStream fs = File.Open(
                        ofdReportFileOpen.FileName, FileMode.Open, FileAccess.Read)) {

                        listCarReports = (BindingList<CarReport>)bf.Deserialize(fs);
                        dgvRecord.DataSource = listCarReports;

                        cbAuthor.Items.Clear();
                        cbCarName.Items.Clear();
                        //�R���{�{�b�N�X�ɓo�^
                        foreach (var report in listCarReports) {
                            setCbAuthor(report.Author);
                            setCbCarName(report.CarName);
                        }
                    }
                }
                catch (Exception) {
                    tsslbMessage.Text = "�t�@�C���`�����Ⴂ�܂�";
                }
            }
        }

        //�t�@�C���Z�[�u����
        private void reportSaveFile() {
            if (sfdReportFileSave.ShowDialog() == DialogResult.OK) {
                try {
                    //�o�C�i���`���ŃV���A����
#pragma warning disable SYSLIB0011
                    var bf = new BinaryFormatter();
#pragma warning restore SYSLIB0011

                    using (FileStream fs = File.Open(
                                    sfdReportFileSave.FileName, FileMode.Create)) {

                        bf.Serialize(fs, listCarReports);
                    }
                }
                catch (Exception ex) {
                    tsslbMessage.Text = "�t�@�C�������o���G���[";
                    MessageBox.Show(ex.Message);//������̓I�ȃG���[���o��
                }
            }
        }

        private void �ۑ�ToolStripMenuItem_Click(object sender, EventArgs e) {
            reportSaveFile(); //�t�@�C���Z�[�u����
        }

        private void �J��ToolStripMenuItem_Click(object sender, EventArgs e) {
            reportOpenFile(); //�t�@�C���I�[�v������
        }

        //�t�H�[����������Ă΂��
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            //�ݒ�t�@�C���֐F����ۑ����鏈���i�V���A�����j
            //P284�ȍ~���Q�l�ɂ���i�t�@�C�����Fsetting.xml�j




        }
    }
}
